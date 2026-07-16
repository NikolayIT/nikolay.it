$(function () {
    const dateTimeFormat = new Intl.DateTimeFormat("bg", {
        weekday: "short",
        day: "numeric",
        month: "short",
        year: "numeric",
        hour: "numeric",
        minute: "2-digit",
    });

    $("time").each(function (i, e) {
        const dateTimeValue = $(e).attr("datetime");
        if (!dateTimeValue) {
            return;
        }

        // Values rendered by the server are UTC but have no timezone designator
        const utcDateTimeValue = /(?:[Zz]|[+-]\d{2}:?\d{2})$/.test(dateTimeValue)
            ? dateTimeValue
            : dateTimeValue + "Z";
        const time = new Date(utcDateTimeValue);
        if (isNaN(time)) {
            return;
        }

        $(e).html(dateTimeFormat.format(time));
        $(e).attr("title", dateTimeValue);
    });
});
