(function () {
  "use strict";

  var instances = new Map();

  function parseList(value) {
    if (!value || !value.trim()) return null;
    return value.split(",").map(function (c) { return c.trim().toLowerCase(); }).filter(Boolean);
  }

  function init(root) {
    if (typeof intlTelInput === "undefined") return;

    root.querySelectorAll(".intl-tel-input-field").forEach(function (container) {
      var display = container.querySelector(".intl-tel-input-display");
      var hidden = container.querySelector('input[type="hidden"]');
      if (!display || !hidden) return;

      var id = container.dataset.fieldId;
      if (instances.has(id)) return;

      var opts = {
        initialCountry: container.dataset.initialCountry || "us",
        separateDialCode: container.dataset.separateDialCode === "true",
        showFlags: container.dataset.showFlags !== "false",
        allowDropdown: container.dataset.allowDropdown !== "false",
        autoPlaceholder: container.dataset.autoPlaceholder || "polite",
        nationalMode: container.dataset.nationalMode !== "false",
        formatOnDisplay: container.dataset.formatOnDisplay !== "false",
        strictMode: container.dataset.strictMode === "true",
        countrySearch: true,
        useFullscreenPopup: false,
        utilsScript: "",
      };

      var preferred = parseList(container.dataset.preferredCountries);
      if (preferred) opts.countryOrder = preferred;

      var only = parseList(container.dataset.onlyCountries);
      if (only) opts.onlyCountries = only;

      var exclude = parseList(container.dataset.excludeCountries);
      if (exclude) opts.excludeCountries = exclude;

      var iti = intlTelInput(display, opts);
      instances.set(id, { iti: iti, container: container, display: display, hidden: hidden });

      if (hidden.value) iti.setNumber(hidden.value);

      display.addEventListener("blur", function () { sync(id); validate(id); });
      display.addEventListener("countrychange", function () { sync(id); });

      var form = container.closest("form");
      if (form && !form.dataset.intlTelInputBound) {
        form.dataset.intlTelInputBound = "1";
        form.addEventListener("submit", function (e) {
          var ok = true;
          instances.forEach(function (_, fid) { sync(fid); if (!validate(fid)) ok = false; });
          if (!ok) { e.preventDefault(); e.stopPropagation(); }
        }, true);
      }
    });
  }

  function sync(id) {
    var i = instances.get(id);
    if (!i) return;
    i.hidden.value = i.iti.getNumber() || "";
  }

  function validate(id) {
    var i = instances.get(id);
    if (!i) return true;

    var value = i.display.value.trim();
    var message = i.container.dataset.validationMessage || "Please enter a valid phone number";

    i.container.classList.remove("intl-tel-input-error");
    var existing = i.container.parentNode.querySelector(".intl-tel-input-error-message");
    if (existing) existing.remove();

    if (!value) return !i.hidden.hasAttribute("data-val-required");

    if (!i.iti.isValidNumber()) {
      i.container.classList.add("intl-tel-input-error");
      var el = document.createElement("span");
      el.className = "field-validation-error intl-tel-input-error-message";
      el.textContent = message;
      i.container.parentNode.appendChild(el);
      return false;
    }
    return true;
  }

  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", function () { init(document); });
  } else {
    init(document);
  }

  window.UmbracoIntlTelInput = { init: init, instances: instances };
})();
