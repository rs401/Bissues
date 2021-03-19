// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function goBack()
{
    window.history.back();
}

function shareThis() 
{
    /* Get the text field */
    var copyText = document.getElementById("PermaLink");

    /* Select the text field */
    copyText.select();
    copyText.setSelectionRange(0, 99999); /* For mobile devices */

    /* Copy the text inside the text field */
    document.execCommand("copy");

    /* Alert the copied text */
    // alert("Copied the text: " + copyText.value);
    copySuccess();
}

function copySuccess()
{
    var success = document.getElementById("CopySuccess");
    var share = document.getElementById("ShareThis");
    success.classList.add("show");
    share.classList.remove("show");
}