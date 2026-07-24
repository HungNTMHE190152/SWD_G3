var index = typeof index !== "undefined" ? index : 0;
$("#btnAdd").click(function () {

    let row = `

<tr>

<td>

<input
name="Criteria[${index}].CriterionName"
class="form-control"/>

</td>

<td>

<input
name="Criteria[${index}].Description"
class="form-control"/>

</td>

<td>

<input
type="number"
step="0.01"
name="Criteria[${index}].MaxScore"
class="form-control"/>

</td>

<td>

<input
type="number"
value="${index + 1}"
name="Criteria[${index}].DisplayOrder"
class="form-control"/>

</td>

<td>

<button
type="button"
class="btn btn-danger btnRemove">

X

</button>

</td>

</tr>

`;

    $("#criteriaTable tbody").append(row);

    index++;

});

$(document).on("click", ".btnRemove", function () {

    $(this).closest("tr").remove();

});