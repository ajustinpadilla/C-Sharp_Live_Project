



// Script for shrinking logo
window.onscroll = function () { shrinkFunction() };

//Simple welcome message that prints to the console on App Start
console.log("Welcome to the theatre!");

//Script for Landing Page
if (document.getElementById("main-carousel") != null) {
    var slideIndex = 1;

    showSlides(slideIndex);

    function changeSlide(n) {
        showSlides(slideIndex += n);
    }

    function currentSlide(n) {
        showSlides(slideIndex = n);
    }

    function showSlides(n) {
        var i;
        var slides = document.getElementsByClassName("slides");
        var dots = document.getElementsByClassName("dot");
        if (n > slides.length) { slideIndex = 1 }
        if (n < 1) { slideIndex = slides.length }
        for (i = 0; i < slides.length; i++) {
            slides[i].style.display = "none";
        }
        for (i = 0; i < dots.length; i++) {
            dots[i].className = dots[i].className.replace(" active", "");
        }
        slides[slideIndex - 1].style.display = "block";
        dots[slideIndex - 1].className += " active";
    }
};

//Script for ~/Photo/Index modal

function ShowModel(id) {


    var modal = document.getElementById("photo-modal-" + id);

    var img = document.getElementById("photo-index-img-" + id);
    var modalImg = document.getElementById("photo-modal--content-" + id);


    modal.style.display = "block";
    modalImg.src = img.src;

    var span = document.getElementById("photo-modal--close-" + id);
    span.onclick = function () {
        modal.style.display = "none";
    }
}



//End script for ~/Photo/Index modal


////Script for sticky navbar
//window.onscroll = function () { stickyNav() };
//var menu = document.getElementById("menu");
//var sticky = menu.offsetTop;

//function stickyNav() {
//    if (window.pageYOffset >= sticky) {
//        menu.classList.add("sticky")
//    } else {
//        menu.classList.remove("sticky");
//    }
//}


function shrinkFunction() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.getElementById("logo").style.height = "40px";
        document.getElementById("menu").style.padding = " 1px 20px";
    } else {
        document.getElementById("logo").style.height = "90px";
        document.getElementById("menu").style.padding = "20px";
    }
}

// Begin script for Bulk Add

if (document.getElementById("generate-showtimes-section") != null) {
    var masterList = [];                                // masterList is used to store the complete list of calendar event objects, as they are added from eventList. It's then passed to the back end.
    var runtime = 0                                     // runtime stores the length of a given event in minutes. It's then used to incrememnt the event's time and create a second event that marks the end time of the production.

    $("#generate__production-field").change(function () {          //when a different production is selected from the dropdown, an ajax call is made getting that production's start date, end date, productionId, and runtime.
        var productionId = $("#generate__production-field").val();
        $.ajax({
            method: 'GET',
            url: '/CalendarEvents/GetDates',
            data: { "productionId": productionId },
            dataType: 'json',
            success: function (data) {
                if (data != "[]") {
                    let production = jQuery.parseJSON(data);
                    let openingDay = production[0].OpeningDay.substr(0, 10); // Removes the time from the date
                    let closingDay = production[0].ClosingDay.substr(0, 10);
                    $("#generate__start-date-field").val(openingDay); //code for adding a date to the start date field
                    $("#generate__end-date-field").val(closingDay);
                    $("#matinee-time").html(moment(production[0].ShowtimeMat).format('h:mm a'));
                    $("#evening-time").html(moment(production[0].ShowtimeEve).format('h:mm a'));
                    runtime = production[0].Runtime;
                }
            },
            error: function () {
                alert("Error while retrieving data!");
            }
        });
    });

    //====================================== This block handles generating, editing and submitting showtimes =====================================//

    $("#generate-button").click(function () {
        $('.bulk-add_review-row').unbind('mouseenter mouseleave');

        var modal = $('#bulk-add-modal'),
            yesBtn = $('#bulk-add-modal_yes'),
            noBtn = $('#bulk-add-modal_no'),
            reviewShowtimes = false,                    //this variable is used to determine whether to have createTable() render the table in the modal or in the 'review showtimes' section.
            eventList = generateShowtimes();
        if (eventList.length < 1) {
            return;
        }
        modal.show();                                  //a modal appears when the generate button is clicked
        createTable(eventList, masterList, reviewShowtimes);       //a table is rendered in the modal with the showtimes the user specified

        noBtn.off('click');                             //the .off() and .one() methods are to prevent event handlers from stacking up.
        noBtn.one("click", function () {
            console.log('no button clicked');
            modal.hide();
            $('.bulk-add_modal-row').remove();          //this clears all the entries from the modal table so they won't stack.
        });
        yesBtn.off("click");
        yesBtn.one("click", function () {               //when the yes button is clicked, the modal disappears and clears its entries, and the showtimes are appended to the review showtimes list. 
            modal.hide();
            reviewShowtimes = true;
            $('.bulk-add_modal-row').remove();
            $('.bulk-add_review-row').remove();
            masterList.push.apply(masterList, eventList); //the event list is appended to the master list
            masterList = masterList.sort((a, b) => a.StartDate - b.StartDate);
            createTable(eventList, masterList, reviewShowtimes);      //the event list is appended to the "review showtimes" table 

        });

        // this function returns a list of show times
        function generateShowtimes() {

            let startDate = moment($("#generate__start-date-field").val()),
                endDate = moment($("#generate__end-date-field").val()),
                eventDate = startDate,
                dateRange = endDate.diff(startDate, 'days'),
                interval = $("#interval").children("option").filter(":selected").val(),
                eventList = [],
                startTimes = [];
            if ($('#matinee').is(':checked')) {
                startTimes.push($('#matinee-time').text());
            }
            if ($('#evening').is(':checked')) {
                startTimes.push($('#evening-time').text());
            }
            if ($('#custom-time').val() != "") {
                startTimes.push($('#custom-time').val());
            }
            if (startTimes.length == 0) {
                alert("Please select a start time");
            }

            let productionDays = [];
            if ($('#sunday').is(':checked')) {
                productionDays.push(0);
            }
            if ($('#monday').is(':checked')) {
                productionDays.push(1);
            }
            if ($('#tuesday').is(':checked')) {
                productionDays.push(2);
            }
            if ($('#wednesday').is(':checked')) {
                productionDays.push(3);
            }
            if ($('#thursday').is(':checked')) {
                productionDays.push(4);
            }
            if ($('#friday').is(':checked')) {
                productionDays.push(5);
            }
            if ($('#saturday').is(':checked')) {
                productionDays.push(6);
            }
            if (productionDays.length == 0) {
                alert("Please select at least one day")
                return (eventList);
            }



            // CalendarEvent properties that are capitalized match the properties in the MVC CalendarEvent model. 
            // They must be verbatim in order for the JSON deserializer to work correctly
            class CalendarEvent {
                constructor(startDate, endDate, dayOfWeek, startTime) {
                    this.Title = $("#generate__production-field").children("option").filter(":selected").text();
                    this.ProductionId = $("#generate__production-field").val();
                    this.StartDate = startDate;
                    this.EndDate = endDate;
                    this.dayOfWeek = dayOfWeek;
                    this.startTime = startTime;
                }
            }
            // this block generates the events
            for (i = 0; i < productionDays.length; i++) {
                if (productionDays[i] < startDate.day()) {
                    productionDays[i] += 7;
                }
                startDate.day(productionDays[i]);
                eventDate = startDate;
                for (j = productionDays[i]; j <= dateRange + 7; j += 7 * interval) {
                    if (eventDate.isBetween(startDate, endDate, undefined, '[]')) { //check for the eventDate to be within start and end date. The '[]' argument sets it to be inclusive of the start and end date.
                        for (k = 0; k < startTimes.length; k++) {
                            let hr = parseInt(startTimes[k].substr(0, startTimes[k].indexOf(':'))),
                                min = parseInt(startTimes[k].substr(startTimes[k].indexOf(':') + 1, 2)),
                                amOrPm = startTimes[k].slice(-2).toUpperCase();
                            if (amOrPm == 'PM' && hr < 12) {
                                hr += 12;
                            }
                            eventDate.hour(hr).minute(min);
                            var endTime = moment(eventDate);
                            endTime.add(runtime, "minutes")
                            const event = new CalendarEvent(moment(eventDate), endTime, eventDate.format('dddd'), startTimes[k]);
                            eventList.push(event);
                        }
                    }
                    eventDate.add((7 * interval).toString(), 'days').format('ll');
                }
                startDate = moment($("#generate__start-date-field").val());
                eventDate = startDate;
            }
            return eventList.sort((a, b) => a.StartDate - b.StartDate);
        }


        //this function generates a table displaying the list of events created in the generateShowTimes() function.
        //depending on the state of the pressedYes variable, it will create the table in either the modal or the 'review showtimes' section.
        function createTable(eventList, masterList, pressedYes) {
            // this block creates a table in the modal
            if (pressedYes != true) {
                var table = document.getElementById("modal-table"),
                    row = table.insertRow();
                row.className = 'bulk-add_modal-row';
                for (i = 0; i < eventList.length; i++) {
                    var cell = row.insertCell();
                    cell.innerHTML = eventList[i].StartDate.format('ll');
                    cell = row.insertCell();
                    cell.innerHTML = eventList[i].dayOfWeek;
                    cell = row.insertCell();
                    cell.innerHTML = eventList[i].startTime;
                    row = table.insertRow();
                    row.className = 'bulk-add_modal-row';
                }
                document.getElementById('bulk-add-modal_content').appendChild(table);
            }

            // this block creates a table in the review showtimes section
            if (pressedYes == true) {
                $("#review-showtimes-section").show();
                var table = document.getElementById("showtimes-table"),
                    row = table.insertRow();
                row.className = 'bulk-add_review-row';
                for (i = 0; i < masterList.length; i++) {
                    var cell = row.insertCell();
                    if (typeof masterList[i].StartDate != "string") {
                        cell.innerHTML = masterList[i].StartDate.format('ll');
                    }
                    cell = row.insertCell();
                    cell.innerHTML = masterList[i].dayOfWeek;
                    cell = row.insertCell();
                    cell.innerHTML = masterList[i].startTime;
                    row = table.insertRow();
                    row.className = 'bulk-add_review-row';
                }
                document.getElementById('showtimes-container').appendChild(table);    // generates the table in html.
                deleteRowFeature();
            }
        }

        // this function creates a delete button when a row is hovered over.
        // When it's clicked, it removes its row, and deletes the event from the master list
        function deleteRowFeature() {
            let row = $('.bulk-add_review-row');
            row.off('hover');                           // clears hover event handlers. prevents events from stacking
            row.hover(function () {                     // when a row is hovered over, a delete button is created, and the index of that row is recorded and used to remove that entry from the master list
                let button = $('<button type="submit" class="bulk-add_delete">Delete</button>')
                    .hide().fadeIn(1200);
                let rowIndex = $('tr').index(this) - 2; // targets the specific row to be deleted 
                console.log('row index' + rowIndex);
                $(this).append(button);
                button.click(function () {             // when the delete button is clicked, the row is removed from the table, and the corresponding event is removed from the master list.
                    button.closest('tr').remove();
                    masterList.splice(rowIndex, 1);
                })
            }, function () {                          // this removes the delete button when the mouse stops hovering over that row.
                $('.bulk-add_delete').remove();
            });
        }
        $('#bulk-add_submit').off('click');
        $('#bulk-add_submit').click(submitEvents);

        function submitEvents() {
            for (var i = 0; i < masterList.length; i++) {
                if (typeof masterList[i].StartDate == "object") {    //this checks that .format is only applied to items that haven't yet been formatted.
                    masterList[i].StartDate = masterList[i].StartDate.format('lll');
                    masterList[i].EndDate = masterList[i].EndDate.format('lll');
                }
            }
            var data = JSON.stringify(masterList);
            console.log('submitted masterlist' + data);
            $.ajax({
                method: 'POST',
                url: '/CalendarEvents/BulkAdd',
                data: { 'jsonString': data },
                success: function () {
                    if (masterList == [""]) {
                        alert("Events already added")
                    }
                    else {
                    alert('Events Added!');
                    masterList = [""];
                    }
                },
                error: function () {
                    alert("Error while posting data!");
                }
            });
        };

    });
}
    //End script for Bulk Add