var accomodationVenueSearchViewModel = function () {
    this.$type = 'accomodationVenueSearchViewModel';
    var root = this;
    this.getAccomodationVenues = function() {
        var params = {
            limit: this.pager.limit(),
            startIndex: this.pager.limit() * (this.pager.page() - 1)
        };

        $.ajax({
            type: 'GET',
            url: '/accomodation/',
            data: {
                q: root.searchTerm(),
                l: root.locationId(),
                e: root.eventId(),
                start:params.startIndex, 
                limit:params.limit
            },
            context: this,
            success: function (data) {
                this(data.Items);
                this.pager.totalCount(data.TotalCount);
            },
            dataType: 'json'
        });
    };

    this.items = ko.observableArray([]);
    this.itemSource = this.items.extend({
        datasource: this.getAccomodationVenues,
        pager: {
            limit: 10
        }
    });
    this.searchTerm = ko.observable("");
    this.eventId = ko.observable(0);

    this.locationId = ko.observable(0);
    this.location = ko.observable("All cities");
};

var accomodationVenueViewModel = function (data) {
    this.$type = 'accomodationVenueViewModel';
    var self = this;
    this.Id = ko.observable();
    this.LatLng = ko.observable();
    this.Address = ko.observable();
    this.Location = ko.observable();
    this.Owner = ko.observable();
    this.DateAdded = ko.observable();
    this.Features = ko.observableArray();
    this.AllowsBedSharing = ko.observable();
    this.AllowsMixedRooms = ko.observable();
    this.Photos = ko.observableArray();
    this.Rooms = ko.observableArray();
    this.Residents = ko.observableArray();
    this.BedCount = ko.computed(function () {
        var sum = 0;
        var b = ko.utils.arrayForEach(self.Rooms(), function (i) {
            if (!i.IsDeleted())
                sum += i.Beds().length;
        })
        return sum;
    });
    if (data)
    {
        ko.mapping.fromJS(data, mapping, this);
    }

    this.addRoom = function () {
        var n = new accomodationRoomViewModel();
        n.Name("Room " + (this.Rooms().length + 1));
        this.Rooms.push(n);
        return n;
    };

    this.addResident = function () {
        var n = new residentViewModel();
        n.ResidentImage(Math.floor(Math.random() * 3 ) + 1);
        this.Residents.push(n);
        return n;
    }
};
var accomodationRoomViewModel = function (data) {
    this.$type = 'accomodationRoomViewModel';
    this.Id = ko.observable();
    this.Name = ko.observable();
    this.AccomodationVenue = ko.observable();
    this.Beds = ko.observableArray();
    this.Features = ko.observableArray();
    this.DateAdded = ko.observable();
    this.Photos = ko.observableArray();
    this.IsDeleted = ko.observable();

    if (data) {
        ko.mapping.fromJS(data, mapping, this);
    }

    this.addBed = function () {
        var bed = new bedViewModel();
        //bed.Size(new bedSizeModel(2, ''));
        //bed.Type(new bedTypeModel(2, ''));
        this.Beds.push(bed);
    };

    this.removeRoom = function () {
        this.IsDeleted(!this.IsDeleted());
    }
};
var bedViewModel = function (data) {
    var self = this;
    this.$type = 'bedViewModel';
    this.Id = ko.observable();
    this.BedSizeId = ko.observable();
    this.BedTypeId = ko.observable();

    this.BedSize = ko.computed(function () {
        var size = ko.utils.arrayFirst(viewModel.AvailableBedSizes(), function (p) {
            return p.Id === self.BedSizeId();
        });
        if (size !== null) {
            return size.Name;
        };
        return "unknown";
    });
    this.BedType = ko.computed(function () {
        var type = ko.utils.arrayFirst(viewModel.AvailableBedTypes(), function (p) {
            return p.Id === self.BedTypeId();
        });
        if (type !== null) {
            return type.Name;
        };
        return "unknown";
    });

    if (data) {
        ko.mapping.fromJS(data, mapping, this);
    }
};

var bedTypeModel = function (id, name) {
    this.$type = 'bedTypeModel';
    this.Id = id;
    this.Name = name;
};
var bedSizeModel = function (id, name) {
    this.$type = 'bedSizeModel';
    this.Id = id;
    this.Name = name;
};
var interestModel = function (id, name) {
    this.$type = 'interestModel';
    this.Id = id;
    this.Name = name;
}

var userProfileViewModel = function (data) {
    var self = this;
    this.$type = 'userProfileViewModel';
    this.AvailableInterests = ko.observableArray();
    this.UserInterests = ko.observableArray();

    if (data) {
        ko.mapping.fromJS(data, mapping, this);
    }
}

var residentViewModel = function (data) {
    var self = this;
    this.$type = 'residentViewModel';
    this.Name = ko.observable();
    this.Order = ko.observable();
    this.ResidentImage = ko.observable();
    this.ResidentCssClass = ko.computed(function () {
        return "resident-" + self.ResidentImage();
    })

    if (data) {
        ko.mapping.fromJS(data, mapping, this);
    }

    this.nextImage = function () {
        this.ResidentImage(((this.ResidentImage() + 1) % 3) + 1);
    }
}

var photoViewModel = function (data) {
    var self = this;
    this.$type = 'photoViewModel';
    this.Id = ko.observable();
    this.Caption = ko.observable();
    this.Url = ko.observable();

    if (data) {
        ko.mapping.fromJS(data, mapping, this);
    }
}


var mapping = {
    'Beds': {
        create: function (options) {
            return new bedViewModel(options.data);
        }
    },
    'Rooms': {
        create: function (options) {
            return new accomodationRoomViewModel(options.data);
        }
    },
    'AvailableBedTypes': {
        create: function (options) {
            return new bedTypeModel(options.data.Id, options.data.Name);
        }
    },
    'AvailableBedSizes': {
        create: function (options) {
            return new bedSizeModel(options.data.Id, options.data.Name);
        }
    },
    'Interests': {
        create: function (options)
        {
            return new interestModel(options.data.Id, options.data.Name);
        }
    },
    'Residents': {
        create: function (options) {
            return new residentViewModel(options.data);
        }
    },
    'Photos': {
        create: function (options) {
            return new photoViewModel(options.data);
        }
    }
};



var saveForm = function (e) {
    var url = e.action;
    var method = e.method;
    var rvt = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        headers: { 'X-RequestVerificationToken': rvt },
        method: method,
        contentType: 'application/json',
        dataType: 'json',
        url: url,
        data: ko.mapping.toJSON(viewModel),
    }).done(function (data) {
        ko.mapping.fromJS(data, viewModel);
    });
    return false;
};