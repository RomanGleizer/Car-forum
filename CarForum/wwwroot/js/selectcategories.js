const brandSelect = document.getElementById("brand");
const modelSelect = document.getElementById("model");
const yearSelect = document.getElementById("year");
const transmissionSelect = document.getElementById("transmission_types");
const bodySelect = document.getElementById("body_types");
const carsDataPath = '/js/cars.json';

let selectedBrand;

const loadJsonData = async (jsonPath) => {
    try {
        const response = await fetch(jsonPath);
        return await response.json();
    } catch (error) {
        console.error(`Error loading JSON data from ${jsonPath}:`, error);
        return null;
    }
};

const addOption = (select, value) => {
    const option = document.createElement("option");
    option.value = value;
    option.text = value;
    select.add(option);
};

const loadOptionsForSelect = async (select, jsonPath, key) => {
    const data = await loadJsonData(jsonPath);

    if (data && data[key]) {
        select.innerHTML = "";
        data[key].forEach(item => {
            addOption(select, item);
        });
    }
};

const updateModels = async (data) => {
    const selectedBrand = brandSelect.value;
    modelSelect.options.length = 0;

    const selectedBrandData = data?.brands.find(brand => brand.name === selectedBrand);

    if (selectedBrandData) {
        selectedBrandData.models.forEach(model => {
            addOption(modelSelect, model);
        });

        if (modelSelect.value !== "") {
            await loadYears();

            fetch('/js/transmission_types.json')
                .then(response => response.json())
                .then(data => populateDropdown('transmission_types', data));

            fetch('/js/body_types.json')
                .then(response => response.json())
                .then(data => populateDropdown('body_types', data));
        }
    }
};

const loadYears = async () => {
    const yearsJsonPath = '/js/years.json';
    const data = await loadJsonData(yearsJsonPath);

    if (data) {
        yearSelect.innerHTML = "";
        data.years.forEach(year => {
            addOption(yearSelect, year);
        });
    }
};

const fetchData = async () => {
    try {
        const response = await fetch(carsDataPath);
        const data = await response.json();

        data.brands.forEach(brand => {
            addOption(brandSelect, brand.name);
        });

        brandSelect.addEventListener('change', async () => {
            if (brandSelect.value !== selectedBrand) {
                selectedBrand = brandSelect.value;
                await updateModels(data);
            }
        });

        modelSelect.addEventListener('change', async () => {
            await loadYears();
        });

        updateModels(data);
    } catch (error) {
        console.error('Error loading JSON data:', error);
    }
};

function populateDropdown(selectId, data) {
    const select = document.getElementById(selectId);

    data.forEach(option => {
        const optionElement = document.createElement('option');
        optionElement.value = option.id;
        optionElement.textContent = option.name;
        select.appendChild(optionElement);
    });
}

fetchData();