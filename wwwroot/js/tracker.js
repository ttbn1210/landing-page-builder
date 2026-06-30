(function () {
    'use strict';

    var config = window.__TRACKQR__;
    if (!config || !config.sessionKey || !config.apiBase) return;

    var sessionKey = config.sessionKey;
    var apiBase = config.apiBase;
    var scrollTracked50 = false;
    var scrollTracked100 = false;

    function sendEvent(eventType, elementName, value) {
        var payload = {
            sessionKey: sessionKey,
            eventType: eventType,
            elementName: elementName || null,
            value: value || null
        };

        if (navigator.sendBeacon) {
            navigator.sendBeacon(
                apiBase + '/api/track/event',
                new Blob([JSON.stringify(payload)], { type: 'application/json' })
            );
        } else {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', apiBase + '/api/track/event', true);
            xhr.setRequestHeader('Content-Type', 'application/json');
            xhr.send(JSON.stringify(payload));
        }
    }

    // Track PageView
    sendEvent('PageView', document.title, window.location.href);

    // Track Button Clicks
    document.addEventListener('click', function (e) {
        var target = e.target.closest('[data-track="button-click"], a.cta-btn, button:not([type="submit"])');
        if (target) {
            var elementName = target.getAttribute('data-element') || target.textContent.trim().substring(0, 50);
            sendEvent('ButtonClick', elementName, target.href || '');
        }
    });

    // Track Call Clicks
    document.addEventListener('click', function (e) {
        var target = e.target.closest('[data-track="call-click"], a[href^="tel:"]');
        if (target) {
            var elementName = target.getAttribute('data-element') || 'phone-call';
            sendEvent('CallClick', elementName, target.href || '');
        }
    });

    // Track Form Submit
    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (form.id === 'leadForm' || form.getAttribute('data-track') === 'form-submit') {
            e.preventDefault();

            var formName = form.getAttribute('data-form-name') || form.id || 'form';
            var formType = form.getAttribute('data-form-type') || 'contact';
            sendEvent('FormSubmit', formName, '');

            // Collect all form data as JSON
            var formData = new FormData(form);
            var formDataObj = {};
            formData.forEach(function (value, key) {
                formDataObj[key] = value;
            });

            // Build lead payload matching API contract
            var leadPayload = {
                sessionKey: sessionKey,
                fullName: formData.get('fullName') || formData.get('name') || '',
                phoneNumber: formData.get('phone') || formData.get('phoneNumber') || '',
                email: formData.get('email') || '',
                formDataJson: JSON.stringify(formDataObj),
                formName: formName,
                formType: formType
            };

            var xhr = new XMLHttpRequest();
            xhr.open('POST', apiBase + '/api/track/lead', true);
            xhr.setRequestHeader('Content-Type', 'application/json');
            xhr.onload = function () {
                // Show success message
                form.innerHTML = '<div style="text-align:center;padding:20px;color:#28a745;font-size:1.2rem;">✓ Cảm ơn bạn! Chúng tôi sẽ liên hệ sớm nhất.</div>';
            };
            xhr.send(JSON.stringify(leadPayload));
        }
    });

    // Track Form Open (focus on first input)
    var formInputs = document.querySelectorAll('#leadForm input, #leadForm textarea');
    var formOpenTracked = false;
    formInputs.forEach(function (input) {
        input.addEventListener('focus', function () {
            if (!formOpenTracked) {
                formOpenTracked = true;
                sendEvent('FormOpen', 'leadForm', '');
            }
        });
    });

    // Track Scroll Depth
    window.addEventListener('scroll', function () {
        var scrollHeight = document.documentElement.scrollHeight - window.innerHeight;
        if (scrollHeight <= 0) return;

        var scrollPercent = (window.scrollY / scrollHeight) * 100;

        if (!scrollTracked50 && scrollPercent >= 50) {
            scrollTracked50 = true;
            sendEvent('Scroll50', 'page', Math.round(scrollPercent) + '%');
        }

        if (!scrollTracked100 && scrollPercent >= 95) {
            scrollTracked100 = true;
            sendEvent('Scroll100', 'page', '100%');
        }
    });

})();
