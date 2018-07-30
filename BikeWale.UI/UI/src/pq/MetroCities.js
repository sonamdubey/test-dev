function MetroCities(cmb)
{
    var optionCount =$(cmb).find("option").length - 1;
    var count = 0;
    $(cmb).find("option[value='']").after("<option value='-1' disabled>---------------</option>");

    if ($(cmb).find("option[value='40']").length > 0) {
        $(cmb).find("option[value='40']").remove();
        $(cmb).find("option[value='']").after("<option value='40'>Thane</option>")
        count++;
    }

    if ($(cmb).find("option[value='12']").length > 0) {
        $(cmb).find("option[value='12']").remove();
        $(cmb).find("option[value='']").after("<option value='12'>Pune</option>");
        count++;
    }

    if ($(cmb).find("option[value='13']").length > 0) {
        $(cmb).find("option[value='13']").remove();
        $(cmb).find("option[value='']").after("<option value='13'>Navi Mumbai</option>")
        count++;
    }

    if ($(cmb).find("option[value='10']").length > 0) {
        $(cmb).find("option[value='10']").remove();
        $(cmb).find("option[value='']").after("<option value='10'>New Delhi</option>");
        count++;
    }

    if ($(cmb).find("option[value='224']").length > 0) {
        $(cmb).find("option[value='224']").remove();
        $(cmb).find("option[value='']").after("<option value='224'>Noida</option>");
        count++;
    }

    if ($(cmb).find("option[value='1']").length > 0) {
        $(cmb).find("option[value='1']").remove();
        $(cmb).find("option[value='']").after("<option value='1'>Mumbai</option>")
        count++;
    }

    if ($(cmb).find("option[value='198']").length > 0) {
        $(cmb).find("option[value='198']").remove();
        $(cmb).find("option[value='']").after("<option value='198'>Kolkata</option>");
        count++;
    }

    if ($(cmb).find("option[value='105']").length > 0) {
        $(cmb).find("option[value='105']").remove();
        $(cmb).find("option[value='']").after("<option value='105'>Hyderabad</option>");
        count++;
    }

    if ($(cmb).find("option[value='246']").length > 0) {
        $(cmb).find("option[value='246']").remove();
        $(cmb).find("option[value='']").after("<option value='246'>Gurgaon</option>");
        count++;
    }

    if ($(cmb).find("option[value='176']").length > 0) {
        $(cmb).find("option[value='176']").remove();
        $(cmb).find("option[value='']").after("<option value='176'>Chennai</option>");
        count++;
    }

    if ($(cmb).find("option[value='2']").length > 0) {
        $(cmb).find("option[value='2']").remove();
        $(cmb).find("option[value='']").after("<option value='2'>Bangalore</option>");
        count++;
    }

    if ($(cmb).find("option[value='128']").length > 0) {
        $(cmb).find("option[value='128']").remove();
        $(cmb).find("option[value='']").after("<option value='128'>Ahmedabad</option>")
        count++;
    }
    
    if (optionCount <= count || count == 0) {
        $(cmb).find("option[value='-1']").remove();
    }
}