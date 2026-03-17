(function ($) {
  var _accountService = abp.services.app.account;
  var _$modal = $("#TenantChangeModal");
  var _$modalContent = _$modal.find("div.modal-content");
  var l = abp.localization.getSource("TOEICReading4");

  function getModalApi() {
    if (window.bootstrap && window.bootstrap.Modal) {
      return window.bootstrap.Modal.getOrCreateInstance(_$modal[0]);
    }

    return null;
  }

  function showModal() {
    var modalApi = getModalApi();

    if (modalApi) {
      modalApi.show();
      return;
    }

    _$modal.modal("show");
  }

  function hideModal() {
    var modalApi = getModalApi();

    if (modalApi) {
      modalApi.hide();
      return;
    }

    _$modal.modal("hide");
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
        focusFirstInput();
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
