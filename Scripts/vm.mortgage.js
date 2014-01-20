
function AmortizationViewModel() {
    var self = this;
    self.payments = ko.observableArray();
    self.isNotEmpty = ko.computed(function() {
        return this.payments().length > 0;
    }, self);
};
function MortgageViewModel() {
    var self = this;
    self.principal = ko.observable(100000);
    self.annualPercentInterest = ko.observable(6.0);
    self.term = ko.observable(30);
    self.termUnits = ko.observable(1);
    self.monthlyPayment = ko.observable();
    self.amortizationViewModel = new AmortizationViewModel();
    self.submit = function() {
        self.amortizationViewModel.payments.removeAll();
        var data = {
            Principal: self.principal(),
            AnnualPercentInterest: self.annualPercentInterest(),
            Term: self.term(),
            TermUnits: self.termUnits()
        };
        $.post("/api/MonthlyPayment", data, function(results) {
            self.monthlyPayment(results);
        });
        $.post("/api/Amortization", data, function(results) {
            self.amortizationViewModel.payments(results);
        });
    };
};

// Wait until DOM is ready, then bind
$(function () {
    ko.applyBindings(new MortgageViewModel());
});