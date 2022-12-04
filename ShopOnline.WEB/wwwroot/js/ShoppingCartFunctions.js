function MakeUpdateQtyButtonVisible(id, visible) {
    const updateQtyButton = document.querySelector(`button[data-itemId="${id}"]`);

    updateQtyButton.style.display = visible == true ? 'inline-block' : 'none';
}