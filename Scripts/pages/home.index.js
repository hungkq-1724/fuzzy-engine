var $title = $("#articleTitle");
var $txt = $("#articleContent");
var $form = $("#articleForm");
var $err = $("#postError");
var $list = $("#articlesList");
var $count = $("#charCount");

$txt.on("input", function () { $count.text($(this).val().length); });

$form.on("submit", function (e) {
    e.preventDefault();

    var title = $title.val().trim();
    var content = $txt.val().trim();

    if (!title) { $err.text("Tiêu đề không được trống."); return; }
    if (title.length > 200) { $err.text("Tiêu đề tối đa 200 ký tự."); return; }
    if (!content) { $err.text("Nội dung không được trống."); return; }

    var url = $form.attr("action");
    var token = $('input[name="__RequestVerificationToken"]', $form).val();

    $.ajax({
        url: url,
        type: 'POST',
        data: { __RequestVerificationToken: token, title: title, content: content },
        success: function (res) {
            if (res && res.success) {
                window.location.reload()
            } else {
                $err.text(res && res.error ? res.error : "Có lỗi xảy ra.");
            }
        },
        error: function (xhr) { $err.text("Lỗi: " + xhr.status + " " + xhr.statusText); }
    });
});

$(document).on("click", ".btn-delete-article", function (e) {
    e.preventDefault();
    var id = $(this).data("id");
    if (!confirm("Bạn chắc chắn muốn xóa bài viết này?")) return;

    var deleteUrl = $list.data("delete-url");
    var tokenInput = $('input[name="__RequestVerificationToken"]').first();
    var token = tokenInput.val();

    $.ajax({
        url: deleteUrl,
        type: 'POST',
        data: { __RequestVerificationToken: token, id: id },
        success: function (res) {
            if (res && res.success) {
                $('.card[data-id="' + id + '"]').remove();
            } else {
                alert(res && res.error ? res.error : "Không thể xóa bài viết.");
            }
        },
        statusCode: {
            401: function () { alert("Bạn cần đăng nhập."); },
            403: function () { alert("Bạn không có quyền xóa bài này."); }
        },
        error: function (xhr) {
            alert("Lỗi: " + xhr.status + " " + xhr.statusText);
        }
    });
});

$(document).on("click", ".btn-like", function (e) {
    e.preventDefault();
    var $btn = $(this);
    var articleId = $btn.data("id");

    var url = $list.data("like-url");

    $.ajax({
        url: url,
        type: 'POST',
        data: { articleId: articleId },
        success: function (res) {
            if (res && res.success) {
                $btn.toggleClass("active", !!res.liked);
                $btn.find(".like-count").text(res.count);
            } else {
                alert(res && res.error ? res.error : "Không thể thực hiện.");
            }
        },
        statusCode: {
            401: function () { window.location = "/Session/Login"; }
        },
        error: function (xhr) {
            alert("Lỗi: " + xhr.status + " " + xhr.statusText);
        }
    });
});
