/**
 * getInputValue(elementId)
 * @param elementId string
 * @returns value of the input specified by elementId
 */
function getInputValue(elementId) {
    var ip = document.getElementById(elementId);
    if (ip && ip.value !== undefined) {
        return ip.value;
    } else {
        return null;
    }
}
