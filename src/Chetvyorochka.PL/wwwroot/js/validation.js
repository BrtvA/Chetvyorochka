/////Валидация данных

function validate(regex, value) {
    return regex.test(value);
}

function checkLength(minLength, maxLength, valueLength) {
    return (valueLength >= minLength) && (valueLength <= maxLength)
}

function checkField(minLength, maxLength, value, regex, popup, popupRegexText, emptyField) {
    var valueLength = value.length;
    if (checkLength(minLength, maxLength, valueLength)) {
        if (validate(regex, value)) {
            popup.innerText = "";
            popup.style.display = "none";
            return true;
        } else {
            popup.innerText = popupRegexText;
            popup.style.display = "block";
            return false;
        }
    } else {
        if (valueLength == 0 && emptyField) {
            popup.innerText = "";
            popup.style.display = "none";
            return true;
        } else {
            popup.innerText = `Минимальное количество символов должно быть ${minLength}, максимальное - ${maxLength}`;
            popup.style.display = "block";
            return false;
        }
    }
}

function checkFieldNumber(min, max, value, popup, isInt) {
    var intgr = Number.isInteger(value);

    if (isInt && !intgr && value != 0) {
        popup.innerText = "Вводимое число должно быть целым";
        popup.style.display = "block";
        return false;
    }

    if (value >= min && value <= max) {
        popup.innerText = "";
        popup.style.display = "none";
        return true;
    } else {
        popup.innerText = `Вводимое значение должно быть в диапазоне от ${min} до ${max}`;
        popup.style.display = "block";
        return false;
    }
}