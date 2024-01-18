const brandSelect = document.getElementById("brand");
const modelSelect = document.getElementById("model");
const yearSelect = document.getElementById("year");
const transmissionSelect = document.getElementById("transmission_types");
const bodySelect = document.getElementById("body_types");
const driveTypes = document.getElementById("drive_types");
const engineTypes = document.getElementById("engine_types");
const engineCapacity = document.getElementById("engine_capacity");

const modelsDataPath = '/js/models.json';
const brandsDataPath = '/js/brands.json';

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
    if (select) {
        const option = document.createElement("option");
        option.value = value;
        option.text = value;
        select.add(option);
    }
};

const loadOptionsForSelect = async (select, jsonPath, key) => {
    const data = await loadJsonData(jsonPath);

    if (data) {
        select.innerHTML = "";
        data.forEach(item => {
            addOption(select, item[key]);
        });
    }
};
const updateModels = async (data) => {
    const selectedBrand = brandSelect.value;
    modelSelect.options.length = 0;

    const selectedBrandData = data[selectedBrand];

    if (selectedBrandData) {
        selectedBrandData.forEach(model => {
            addOption(modelSelect, model);
        });

        if (modelSelect.value !== "") {
            await loadYears();

            const transmissionData = await loadJsonData('/js/transmission_types.json');
            const bodyData = await loadJsonData('/js/body_types.json');
            const engineTypeData = await loadJsonData('/js/engine_types.json'); 
            const engineCapacityData = await loadJsonData('/js/engine_capacity.json'); 
            const driveTypeData = await loadJsonData('/js/drive_types.json'); 

            populateDropdown('transmission_types', transmissionData);
            populateDropdown('body_types', bodyData);
            populateDropdown('drive_types', driveTypeData);
            populateDropdown('engine_types', engineTypeData);
            populateDropdown('engine_capacity', engineCapacityData);
        }
    } else {
        modelSelect.options.length = 0;
        yearSelect.options.length = 0;
        transmissionSelect.options.length = 0;
        bodySelect.options.length = 0;
        driveTypes.options.length = 0;
        engineTypes.options.length = 0;
        engineCapacity.options.length = 0;
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
        const brandsData = await loadJsonData(brandsDataPath);
        const modelsData = await loadJsonData(modelsDataPath);

        if (brandsData && modelsData && brandSelect) {
            brandsData.forEach(brand => {
                addOption(brandSelect, brand.name);
            });

            brandSelect.addEventListener('change', async () => {
                if (brandSelect.value !== selectedBrand) {
                    selectedBrand = brandSelect.value;
                    await updateModels(modelsData);
                }
            });

            modelSelect.addEventListener('change', async () => {
                await loadYears();
            });

            updateModels(modelsData);
        }
    } catch (error) {
        console.error(error);
    }
};

function populateDropdown(selectId, data) {
    const select = document.getElementById(selectId);

    data.forEach(option => {
        const optionElement = document.createElement('option');
        optionElement.value = option.name;
        optionElement.textContent = option.name;
        select.appendChild(optionElement);
    });
}

fetchData();
