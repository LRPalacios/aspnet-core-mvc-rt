$(function () {
    $('#btn-previous').click(() => {
        $('.comic-panel').addClass('fadeOutRight');
    });
    $('#btn-next').click(() => {
        console.log('click');
        $('.comic-panel').addClass('fadeOutLeft');
    });
});