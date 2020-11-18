function getBrowser() {
  // Opera 8.0+
  var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
  if (isOpera) {
    return 'opera';
  }

  // Firefox 1.0+
  var isFirefox = typeof InstallTrigger !== 'undefined';
  if (isFirefox) {
    return 'firefox';
  }

  // Safari 3.0+ "[object HTMLElementConstructor]" 
  var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) {
    return p.toString() === "[object SafariRemoteNotification]";
  })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));
  if (isSafari) {
    return 'safari';
  }

  // Internet Explorer 6-11
  var isIE = /*@cc_on!@*/ false || !!document.documentMode;
  if (isIE) {
    return 'IE';
  }

  // Edge 20+
  var isEdge = !isIE && !!window.StyleMedia;
  if (isEdge) {
    return 'edge';
  }

  // Chrome 1 - 71
  var isChrome = !!window.chrome;
  if (isChrome) {
    return 'chrome';
  }
}

// Get lower IE 11
function verifyLowerIE11() {
  var version = detectIE();

  return !version || version < 11;
}

function detectIE() {
  var ua = window.navigator.userAgent;

  var msie = ua.indexOf('MSIE ');
  if (msie > 0) {
    // IE 10 or older => return version number
    return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
  }

  var trident = ua.indexOf('Trident/');
  if (trident > 0) {
    // IE 11 => return version number
    var rv = ua.indexOf('rv:');
    return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
  }

  var edge = ua.indexOf('Edge/');
  if (edge > 0) {
    // Edge (IE 12+) => return version number
    return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
  }

  // other browser
  return false;
}

function initializeDetection() {
  var browserName = getBrowser();
  var isIE = !browserName || browserName.trim().toUpperCase() === 'IE';

  if (window.location.href && window.location.href.trim().toLowerCase().indexOf('browser-error') === -1) {
    if (isIE) {
      if (verifyLowerIE11()) {
        location.href = window['base-href'] + 'assets/browser-error.html';
      }
    }
  } else if (isIE && !verifyLowerIE11()) {
    location.href =  window['base-href'];
  }
}

(function (window, undefined) {
  initializeDetection();
})(window);