/////Валидация данных

//let regCyrillic = /^[А-Яа-яЁё]\S+$/;
//let regLatinNumber = /^[A-Za-z0-9]\S+$/;

//let regCyrillicNumberSpace = /^[А-Яа-яЁё0-9]+$/;
//let regCyrillicLatinSpace = /^[A-Za-zА-Яа-яЁё]+$/;

//const popupLatinNumberText = "Используйте только латинские символы и цифры";
//const popupCyrillicText = "Используйте только кириллицу";

//const popupCyrillicLatinSpaceText = "Используйте только кириллицу ,латиницу и пробел";
//const popupCyrillicNumberSpaceText = "Используйте только кириллицу, цифры и пробел";

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