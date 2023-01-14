/////Валидация данных

function validate(regex, value) {
    return regex.test(value);
}

function checkLength(minLength, maxLength, valueLength) {
    if (valueLength >= minLength && valueLength <= maxLength) {
        return true;
    }
    else {
        return false;
    }
}

function checkField(minLength, maxLength, value, regex, popup, popupRegexText) {
    if (checkLength(minLength, maxLength, value.length) == true) {
        if (validate(regex, value) == true) {
            popup.innerText = "";
            popup.style.display = "none";
            return true;
        } else {
            popup.innerText = popupRegexText;
            popup.style.display = "block";
            return false;
        }
    } else {
        popup.innerText = `Минимальное количество символов должно быть ${minLength}, максимальное - ${maxLength}`;
        popup.style.display = "block";
        return false;
    }
}

function checkFieldNumber(min, max, value, popup, isInt) {
    var intgr = Number.isInteger(value);

    if (isInt == true && intgr == false && value != 0) {
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