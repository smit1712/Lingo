import 'jquery';
import $ from 'jquery';

let oldguesses = [];
let guesses;
export async function Init() {

    let SubmitButton = document.getElementById("SubmitButton");
    SubmitButton.onclick = function () { submit() };
    SubmitButton.disabled = true;

    let NewWordButton = document.getElementById("NewWordButton");
    NewWordButton.onclick = function () { NewWord() };
    NewWordButton.hidden = true;

    let Entry = document.getElementById("label")
    Entry.onchange = function () { EntryChanged() };

    $(function () {
        $("#SubmitButton").on("click", function () {
            $("entry").animate({
                backgroundColor: "#00FF00",
                color: "#fff"
            }, 1000);
        });
    });
}

export default async function GetLingoWord(data = null) {

    if (data == null) {
        const response = await fetch('Lingo');
        data = await response.json();
    }

    if (isGuessed(data)) {
        let NewWordButton = document.getElementById("NewWordButton");
        NewWordButton.hidden = false;
    }
    refreshGuesses(data.guessedWord)
}

function refreshGuesses(latestguess) {
    if (guesses == undefined) {
        guesses = document.createElement("div");
        guesses.id = "guesses";
        document.body.appendChild(guesses);
    }
    if (oldguesses != undefined) {
        oldguesses.push(latestguess);
    }
    let div = document.createElement("div");
    div.className = "center"
    let l;
    for (l = 0; l < latestguess.length; l++) {
        let label = document.createElement("label");
        label.className = updateLetterClass(l, latestguess)

        if (label.className == "wrong") {
            label.innerHTML = "#";
        } else {
            label.innerHTML = latestguess[l].character;
        }
        div.append(label);
    }
    guesses.append(div);
}

function updateLetterClass(index, word) {
    if (word[index].letterStatus == 0) {
        return "wrong";
    }
    if (word[index].letterStatus == 1) {
        return "wrongLocation";
    }
    if (word[index].letterStatus == 2) {
        return "correct";
    }
}

function submit() {
    const entry = document.getElementById('entry');

    const item = {
        isComplete: false,
        name: entry.value.trim()
    };

    fetch('Lingo/Guess', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            GetLingoWord();
        })
        .catch(error => console.error('Unable to submit word.', error));
}

function isGuessed(data) {
    if (data.guessedWord.length == 0) return;

    let guessed = true;
    let i = 0;
    for (i = 0; i < 5; i++) {
        if (data.guessedWord[i].letterStatus != 2) {
            guessed = false;
        }
    }
    return guessed;
    return false;
}

async function NewWord() {
    const response = await fetch('Lingo/Reset');
    const data = await response.json();
    GetLingoWord(data);
    let entry = document.getElementById('entry');
    entry.value = "";

    let NewWordButton = document.getElementById("NewWordButton");
    NewWordButton.hidden = true;

    document.body.removeChild(guesses);
    guesses = undefined;
}

function EntryChanged() {
    let Entry = document.getElementById("entry")
    let SubmitButton = document.getElementById("SubmitButton");

    if (Entry.value.length == 5) {
        SubmitButton.disabled = false;
    } else {
        SubmitButton.disabled = true;
    }
}