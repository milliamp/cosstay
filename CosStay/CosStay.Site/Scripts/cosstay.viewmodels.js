
var accomodationVenueViewModel = function (data) {
    this.$type = 'accomodationVenueViewModel';

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

    if (data)
    {
        ko.mapping.fromJS(data, mapping, this);
        }

    this.addRoom = function () {
        var n = new accomodationRoomViewModel();
        this.Rooms.push(n);
        return n;
    };
};
var accomodationRoomViewModel = function (data) {
    this.$type = 'accomodationRoomViewModel';
    this.Id = ko.observable();
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