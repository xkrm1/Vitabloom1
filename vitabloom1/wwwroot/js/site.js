let waterCount = 0;

function addWater() {
    if (waterCount < 8) {
        waterCount++;

        // Add one cup emoji
        document.getElementById("glasses").innerHTML += "🥛 ";

        // Update count
        document.getElementById("waterCount").textContent = waterCount;

        if (waterCount === 8) {
            document.getElementById("waterMessage").textContent =
                "🎉 Great job! You reached your water goal!";
        } else {
            document.getElementById("waterMessage").textContent =
                "💧 Keep going!";
        }
    }
}

function showRecipe(title, icon, description, instructions) {

    document.getElementById("modalTitle").textContent = title;

    document.getElementById("modalIcon").textContent = icon;

    document.getElementById("modalDescription").textContent = description;

    document.getElementById("modalInstructions").textContent = instructions;

    document.getElementById("recipeModal").style.display = "flex";
}


function closeRecipe() {

    document.getElementById("recipeModal").style.display = "none";

}

function likePost(button) {

    let count = button.querySelector("span");

    let likes = parseInt(count.textContent);

    likes++;

    count.textContent = " " + likes;
}

