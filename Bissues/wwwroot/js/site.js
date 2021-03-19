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
    var permaLink = document.getElementById("PermaLink");

    /* Select the text field */
    permaLink.select();
    permaLink.setSelectionRange(0, 99999); /* For mobile devices */

    /* Copy the text inside the text field */
    document.execCommand("copy");

    /* Alert the copied text */
    // alert("Copied the text: " + copyText.value);
    copySuccess();
}

function copySuccess()
{
    var share = document.getElementById("ShareThis");
    var success = document.getElementById("CopySuccess");
    var html = `
        <strong>Success!</strong> Link copied to your clipboard.
        <a type="button" class="close" data-toggle="collapse" href="#CopySuccess" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </a>`;
    success.innerHTML = html;
    share.classList.remove("show");
    success.classList.add("show");
}