/////Валидация данных
let regCyrillic = /^[А-Яа-яЁё]+$/;
let regLatinNumber = /^[A-Za-z0-9]+$/;

const popupLatinNumberText = "Используйте только латинские символы и цифры";
const popupCyrillicText = "Используйте только кириллицу";

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