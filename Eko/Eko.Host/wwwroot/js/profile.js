import { fetchData } from './fetch.js';

// Helper function to safely set content
const setContent = (element, value) => {
    if (element) element.textContent = value || "Не указано";
};

document.addEventListener("DOMContentLoaded", async function () {
    const email = window.userEmail || localStorage.getItem("emailAddress") || "";
    if (!email) {
        console.error("Email is not available");
        return;
    }

    try {
        const person = await fetchData("/Person/TakePerson", "GET", null, {'emailAddress': email}).then(response => response.json());
        console.log("Person data:", person);
        console.log("Person data:", person["skills"]);

        // Set text content for display elements
        setContent(document.getElementById("name"), person["name"]);
        setContent(document.getElementById("specialization"), person["specialization"]);
        setContent(document.getElementById("email"), person["email"]);
        setContent(document.getElementById("name2"), person["name"]);
        setContent(document.getElementById("specialization2"), person["specialization"]);
        setContent(document.getElementById("email2"), person["email"]);
        setContent(document.getElementById("dataofbirth"), person["dataOfBirth"]);
        setContent(document.getElementById("experience"), person["experience"]);
        setContent(document.getElementById("biography"), person["biography"]);

        // Set skills display
        const skillsElement = document.getElementById("skills");
        if (skillsElement) {
            const skillsArray = (person["skills"]).split(',').map(skill => skill.trim()).filter(skill => skill !== "");

            if (skillsArray.length > 0) {
                skillsElement.innerHTML = skillsArray
                    .map(skill => `<span class="bd-badge badge-outline-primary badge-transparent">${skill}</span>`)
                    .join(' ');
            } else {
                skillsElement.textContent = "Не указано";
            }
        }

        // Set form field values
        const phoneInput = document.getElementById("phone");
        if (phoneInput) phoneInput.value = person["phone"] || "";

        const skillsInput = document.getElementById("skills");
        if (skillsInput) skillsInput.value = person["skills"] || "";

    } catch (error) {
        console.error("Error fetching person data:", error);
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector(".app-profile-personal-info");
    const email = window.userEmail || localStorage.getItem("emailAddress") || "";

    if (form) {
        form.addEventListener("submit", async (e) => {
            e.preventDefault();

            try {
                const formData = new FormData(form);
                const data = {
                    specialization: formData.get('specialization'),
                    dateOfBirth: formData.get('dateOfBirth'),
                    experience: formData.get('experience'),
                    biography: formData.get('biography'),
                    skills: formData.get('skills')
                };

                console.log("Form data to be sent:", data); 

                const urlEncoded = new URLSearchParams();
                for (const key in data) {
                    if (data[key] !== null && data[key] !== undefined) {
                        urlEncoded.append(key, data[key]);
                    }
                }

                const headers = {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'emailAddress': email
                };

                await fetchData("/Person/SaveInfo", "POST", urlEncoded.toString(), headers);
                window.location.href = "/";
            } catch (error) {
                console.error("Save failed:", error);
                alert("Save failed. Please try again.");
            }
        });
    }
});