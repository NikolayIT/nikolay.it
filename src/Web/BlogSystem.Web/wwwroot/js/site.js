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

        // Date-only values (blog posts, videos) are already rendered as the date we want to show.
        // Reformatting them here would add a time the post does not have, and converting them to
        // the viewer's timezone could shift them by a day.
        if (/^\d{4}-\d{2}-\d{2}$/.test(dateTimeValue)) {
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

// Syntax highlighting for the code blocks in the posts. The archive already contains
// highlight.js markup (hljs-* spans) from the old editor, but the library was never loaded,
// so every code block rendered as plain text. Highlighting from the text content covers
// those blocks as well as the ones that carry no markup at all.
$(function () {
    if (!window.hljs) {
        return;
    }

    hljs.configure({ languages: ["csharp", "javascript", "xml", "css", "sql", "json", "bash"] });

    document.querySelectorAll(".blog-post-content pre").forEach(function (pre) {
        let code = pre.querySelector("code");
        if (!code) {
            code = document.createElement("code");
            code.textContent = pre.textContent;
            pre.textContent = "";
            pre.appendChild(code);
        }

        // The oldest posts record the language the SyntaxHighlighter way: <pre class="brush: csharp">
        const brush = /brush:\s*([\w-]+)/.exec(pre.className || "");
        if (brush && hljs.getLanguage(brush[1])) {
            code.classList.add("language-" + brush[1]);
        }

        hljs.highlightElement(code);
    });
});
