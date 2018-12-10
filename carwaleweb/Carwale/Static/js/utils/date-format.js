(function () {
    this.fullMonthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    this.abbreviatedMonthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    this.fullDayNames = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];
    this.abbreviatedDayNames = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];

    Object.defineProperty(Date.prototype, "format",
        {
            value: function format(_options) {
                var options = _options.split(/(\/|:|-|,| )/g);
                var formatedDate = '';
                for(var i=0; i < options.length; i++)
                {
                    formatedDate += getDatePart(this, options[i]);
                }
                return formatedDate;
            }
        });

    this.getDatePart = function(date,part)
    {
        switch(part)
        {
            case "d": return date.getDate(); break;
            case "dd": return AppendLeadingZero(date.getDate()); break;
            case "ddd": return abbreviatedDayNames(date.getDay()); break;
            case "dddd": return fullDayNames(date.getDay()); break;
            case "h": return date.getHours()%12; break;
            case "hh": return AppendLeadingZero(date.getHours() % 12); break;
            case "H": return date.getHours(); break;
            case "HH": return AppendLeadingZero(date.getHours()); break;
            case "m": return date.getMinutes(); break;
            case "mm": return AppendLeadingZero(date.getMinutes()); break;
            case "M": return date.getMonth(); break;
            case "MM": return AppendLeadingZero(date.getMonth()); break;
            case "MMM": return abbreviatedMonthName[date.getMonth()]; break;
            case "MMMM": return fullMonthNames[date.getMonth()]; break;
            case "s": return date.getSeconds(); break;
            case "ss": return AppendLeadingZero(date.getSeconds()); break;
            case "t": return date.getHours() < 12 ? 'A':'P'; break;
            case "tt": return date.getHours() < 12 ? 'AM' : 'PM'; break;
            case "y": return date.getFullYear().toString().substr(-2); break;
            case "yy": return date.getFullYear().toString().substr(-3); break;
            case "yyy": return date.getFullYear(); break;
            case "yyyy": return date.getFullYear(); break;
            default: return part; break;;
        }
    }

    this.AppendLeadingZero = function(number)
    {
        return number < 10 ? '0' + number : number;
    }

})();