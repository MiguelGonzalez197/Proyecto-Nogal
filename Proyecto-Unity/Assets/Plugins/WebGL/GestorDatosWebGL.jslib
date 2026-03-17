mergeInto(LibraryManager.library, {
  RegistrarEventosPaginaWebGL: function (nombreObjetoPtr, nombreMetodoPtr) {
    var nombreObjeto = UTF8ToString(nombreObjetoPtr);
    var nombreMetodo = UTF8ToString(nombreMetodoPtr);

    if (!window.__gestorDatosWebGLHandlers) {
      window.__gestorDatosWebGLHandlers = {};
    }

    var llave = nombreObjeto + "::" + nombreMetodo;
    if (window.__gestorDatosWebGLHandlers[llave]) {
      return;
    }

    var callback = function () {
      if (typeof SendMessage === "function") {
        SendMessage(nombreObjeto, nombreMetodo);
      }
    };

    window.addEventListener("pagehide", callback);
    window.addEventListener("beforeunload", callback);
    window.__gestorDatosWebGLHandlers[llave] = callback;
  },

  EnviarMetricasWebGL: function (urlPtr, jsonPtr) {
    var url = UTF8ToString(urlPtr);
    var json = UTF8ToString(jsonPtr);
    var payload = new URLSearchParams();

    payload.append("data", json);

    if (navigator.sendBeacon) {
      navigator.sendBeacon(url, payload);
      return;
    }

    fetch(url, {
      method: "POST",
      body: payload,
      keepalive: true,
      credentials: "omit"
    }).catch(function () {});
  }
});
