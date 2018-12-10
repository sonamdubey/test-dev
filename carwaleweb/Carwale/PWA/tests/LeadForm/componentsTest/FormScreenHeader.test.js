import React from "react";
import FormScreenHeader from "../../../src/LeadForm/Components/FormScreenHeader";

test("Component FormScreenHeader Snapshot RenderedTree (1)", () => {
  let props = {
    isCrossSell: true,
    campaignName: "Shivam Autozone",
    imageUrl:
      "https://imgd.aeplcdn.com/310x174/cw/ec/26860/Maruti-Suzuki-Dzire-Exterior-118637.jpg?wm=0",
    fullCarName: "Maruti Suzuki Dzire"
  };
  let wrapper = shallow(<FormScreenHeader {...props} />);
  expect(wrapper).toMatchSnapshot();
});

test("Component FormScreenHeader Snapshot RenderedTree (2)", () => {
  let props = {
    isCrossSell: false,
    imageUrl: "",
    campaignName: "Shivam Autozone",
    fullCarName: ""
  };

  let wrapper = shallow(<FormScreenHeader {...props} />);
  expect(wrapper).toMatchSnapshot();
});
