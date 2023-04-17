window.showNodes = (visibleNodesIds) => {
    visibleNodesIds.forEach((id) => {
        document.querySelector('[data-node-id="' + id + '"]').style.visibility = 'visible';
    });
}

window.showAllNodes = () => {
    document.querySelectorAll('[data-node-id]').forEach((element) => { element.style.visibility = 'visible'; });
}

window.hideNodes = (hiddenNodesId) => {
    hiddenNodesId.forEach((id) => {
        document.querySelector('[data-node-id="' + id + '"]').style.visibility = 'hidden';
    });
}

window.hideAllNodes = () => {
    document.querySelectorAll('[data-node-id]').forEach((element) => { element.style.visibility = 'hidden'; });
}

window.showLinks = (visibleLinksIds) => {
    visibleLinksIds.forEach((id) => {
        document.querySelector('[data-link-id="' + id + '"]').style.visibility = 'visible';
    });
}

window.showAllLinks = () => {
    document.querySelectorAll('[data-link-id]').forEach((element) => { element.style.visibility = 'visible'; });
}

window.hideLinks = (hiddenLinksId) => {
    hiddenLinksId.forEach((id) => {
        document.querySelector('[data-link-id="' + id + '"]').style.visibility = 'hidden';
    });
}

window.hideAllLinks = () => {
    document.querySelectorAll('[data-link-id]').forEach((element) => { element.style.visibility = 'hidden'; });
}

window.highlightSelectedNode = () => {

}