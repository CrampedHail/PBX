const openModal = document.querySelector('.open-button');
const closeModal = document.querySelector('.close-button');
const modal = document.querySelector('.modal');
const categoryInput = document.querySelector('#show-selected');
const errorLabel = document.querySelector('#error');

openModal.addEventListener('click', () => {
    errorLabel.innerHTML = "";
    modal.showModal();
})
closeModal.addEventListener('click', () => {
    let buttons = document.querySelectorAll('.radio');
    buttons.forEach((button) => {
        if (button.checked) {
            categoryInput.innerHTML = button.value;
            categoryInput.id = button.id;
            modal.close();
        }
    })
    errorLabel.innerHTML = "Należy wybrać kategorię.";
})