Congratulations for making it this far!

Included in this project is skeleton code for a basic MVC ASP.NET site. It was built in Visual Stuio Code, but should be somewhat universal.
This project should only take a couple of hours if you're already familiar with MVC.

XKCD, a popular webcomic, provides a JSON API to their comics. They expose two endpoints:
1) https://xkcd.com/info.0.json will retrieve JSON discribing the most recent comic. 
2) https://xkcd.com/XXX/info.0.json will retrieve comic number XXX

Your task is to build a simple web MVC application that will display today's comic, a given comic, and the next comic.

Task 1
=========
When run, the application should host an ASP.net MVC website.
The root of the application should display today's XKCD comic.
It should show the title, the the comic image, and the alt text.
Appearance/styling is unimportant here. You can use basic HTML elements with no CSS if you like.

The basic site would look like this site in this link: https://s3.amazonaws.com/rain-public/interviews/xkcd-site.png

Task 2
==========
Seeing today's comic is great, but we'd love to see the previous day's comic.
A previous link should be available. Clicking on this link will take you to the previous day's comic.
The URL of each page (except for the landing page) should be /comic/XXX where XXX is the id of that day's comic.
The previous button should only appear if ther actually is a previous comic, so /comic/1 would have no previous button.
Equivalent functionality should be available for a next button.

Task 3
========
Because Randall Munroe is a techie, he left comic 404 empty. That is, it will always 404. When you navigate his comic you'll
notice that it skips from 403 to 405. Your comic viewer should do the same thing. However, do not specially code for this number.
Instead make sure that when navigating to the next or previous comic that you're always only looking for the next comic that can
be successfully retireved.
