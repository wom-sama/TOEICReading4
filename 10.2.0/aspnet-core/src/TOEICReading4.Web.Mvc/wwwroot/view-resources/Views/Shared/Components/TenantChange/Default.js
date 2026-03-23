(function ($) {
  var _accountService = abp.services.app.account;
  var _$modal = $("#TenantChangeModal");
  var _$modalContent = _$modal.find("div.modal-content");
  var l = abp.localization.getSource("TOEICReading4");

  // Move modal outside of .account-stage to escape stacking context
  $(document).ready(function() {
    var modalElement = _$modal[0];
    if (modalElement && modalElement.parentNode) {
      // Move modal to body to escape stacking context from backdrop-filter
      document.body.appendChild(modalElement);
    }
  });

  function getModalApi() {
    if (window.bootstrap && window.bootstrap.Modal) {
      var modalElement = _$modal[0];
      // Remove any existing instance to avoid reuse issues
      if (bootstrap.Modal.getInstance(modalElement)) {
        bootstrap.Modal.getInstance(modalElement).dispose();
      }
      
      // Create new instance with proper options
      return new bootstrap.Modal(modalElement, {
        backdrop: 'static',
        keyboard: false,
        focus: true
      });
    }

    return null;
  }

  function showModal() {
    try {
      var modalApi = getModalApi();

      if (modalApi) {
        // Ensure modal is properly set to show
        modalApi.show();
        return;
      }

      // Fallback to jQuery modal
      _$modal.modal("show");
    } catch (e) {
      console.error("Error showing tenant change modal:", e);
      abp.notify.error(l("CouldNotOpenTenantSelector"));
    }
  }

  function hideModal() {
    try {
      var modalApi = getModalApi();

      if (modalApi) {
        modalApi.hide();
        return;
      }

      // Fallback to jQuery modal
      _$modal.modal("hide");
    } catch (e) {
      console.error("Error hiding tenant change modal:", e);
    }
  }

  function getForm() {
    return _$modal.find("form[name=TenantChangeForm]");
  }

  function focusFirstInput() {
    var _$firstInput = getForm().find("input[type=text]:first");

    if (_$firstInput.length) {
      window.setTimeout(function () {
        _$firstInput.trigger("focus");
      }, 150);
    }
  }

  function switchToSelectedTenant() {
    var _$form = getForm();
    var tenancyName = _$form.find("input[name=TenancyName]").val();

    if (!tenancyName) {
      hideModal();
      abp.multiTenancy.setTenantIdCookie(null);
      location.reload();
      return;
    }

    _accountService
      .isTenantAvailable({
        tenancyName: tenancyName,
      })
      .done(function (result) {
        switch (result.state) {
          case 1:
            hideModal();
            abp.multiTenancy.setTenantIdCookie(result.tenantId);
            location.reload();
            return;
          case 2:
            abp.message.warn(
              abp.utils.formatString(l("TenantIsNotActive"), tenancyName),
            );
            return;
          case 3:
            abp.message.warn(
              abp.utils.formatString(
                l("ThereIsNoTenantDefinedWithName{0}"),
                tenancyName,
              ),
            );
            return;
        }
      });
  }

  $(document).on("click", ".tenant-change-link", function (e) {
    e.preventDefault();

    abp.ajax({
      url: abp.appPath + "Account/TenantChangeModal",
      type: "POST",
      dataType: "html",
      success: function (content) {
        _$modalContent.html(content);
        showModal();
        // Add delay to ensure modal is fully rendered before focusing
        window.setTimeout(function () {
          focusFirstInput();
        }, 150);
      },
      error: function () {
        abp.notify.error(l("CouldNotOpenTenantSelector"));
      },
    });
  });

  $(document).on("click", "#TenantChangeModal .save-button", function (e) {
    e.preventDefault();
    switchToSelectedTenant();
  });

  $(document).on("keypress", "#TenantChangeModal input", function (e) {
    if (e.which === 13) {
      e.preventDefault();
      switchToSelectedTenant();
    }
  });

  _$modal.on("hidden.bs.modal", function () {
    _$modalContent.empty();
  });
})(jQuery);
