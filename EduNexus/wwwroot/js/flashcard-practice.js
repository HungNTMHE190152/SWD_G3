const flashcards = window.flashcardPracticeData || [];

let currentIndex = 0;

const practiceResults = new Array(flashcards.length).fill(null);

const card = document.getElementById("practiceCard");
const flipButton = document.getElementById("flipButton");
const previousButton = document.getElementById("previousButton");
const nextButton = document.getElementById("nextButton");
const correctButton = document.getElementById("correctButton");
const incorrectButton = document.getElementById("incorrectButton");

const frontContent = document.getElementById("frontContent");
const backContent = document.getElementById("backContent");

const progressText = document.getElementById("progressText");
const progressBar = document.getElementById("progressBar");

const savingOverlay = document.getElementById("savingOverlay");

function renderCard() {

    const current = flashcards[currentIndex];

    frontContent.textContent = current.FrontContent;
    backContent.textContent = current.BackContent;

    progressText.textContent =
        `${currentIndex + 1} / ${flashcards.length}`;

    progressBar.style.width =
        `${((currentIndex + 1) / flashcards.length) * 100}%`;

    card.classList.remove("flipped");

    flipButton.textContent = "Flip Card";

    previousButton.disabled = currentIndex === 0;
    nextButton.disabled = currentIndex === flashcards.length - 1;

    updateAnswerButtons();
}

function updateAnswerButtons() {

    correctButton.classList.remove("selected-correct");
    incorrectButton.classList.remove("selected-incorrect");

    const result = practiceResults[currentIndex];

    if (result === true) {
        correctButton.classList.add("selected-correct");
    }

    if (result === false) {
        incorrectButton.classList.add("selected-incorrect");
    }
}

flipButton.addEventListener("click", () => {

    card.classList.toggle("flipped");

    flipButton.textContent =
        card.classList.contains("flipped")
            ? "Show Front"
            : "Flip Card";
});

previousButton.addEventListener("click", () => {

    if (currentIndex > 0) {

        currentIndex--;

        renderCard();

    }

});

nextButton.addEventListener("click", () => {

    if (currentIndex < flashcards.length - 1) {

        currentIndex++;

        renderCard();

    }

});

correctButton.addEventListener("click", () => {

    saveAnswer(true);

});

incorrectButton.addEventListener("click", () => {

    saveAnswer(false);

});

function saveAnswer(isCorrect) {

    practiceResults[currentIndex] = isCorrect;

    updateAnswerButtons();

    if (currentIndex < flashcards.length - 1) {

        currentIndex++;

        renderCard();

    }
    else {

        finishPractice();

    }

}

async function submitPractice() {

    savingOverlay.classList.remove("d-none");

    const answers = [];

    flashcards.forEach((card, index) => {

        if (practiceResults[index] !== null) {

            answers.push({

                flashcardId: card.FlashcardId,

                isCorrect: practiceResults[index]

            });

        }

    });

    try {

        const response = await fetch("/FlashcardPractice/SavePractice", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                answers: answers
            })

        });

        if (!response.ok) {

            alert("Failed to save practice.");

            return false;

        }

        return true;

    }
    catch (error) {

        console.error(error);

        alert("Unable to connect to the server.");

        return false;

    }
    finally {

        savingOverlay.classList.add("d-none");

    }

}

async function finishPractice() {

    const success = await submitPractice();

    if (!success) {
        return;
    }

    const total = practiceResults.length;

    const correct =
        practiceResults.filter(x => x === true).length;

    const incorrect =
        practiceResults.filter(x => x === false).length;

    const score =
        total === 0
            ? 0
            : Math.round(correct / total * 100);

    document
        .getElementById("practiceArea")
        .classList.add("d-none");

    document
        .getElementById("resultArea")
        .classList.remove("d-none");

    document
        .getElementById("resultCorrect")
        .textContent = correct;

    document
        .getElementById("resultIncorrect")
        .textContent = incorrect;

    document
        .getElementById("resultTotal")
        .textContent = total;

    document
        .getElementById("resultScore")
        .textContent = score + "%";

}

renderCard();