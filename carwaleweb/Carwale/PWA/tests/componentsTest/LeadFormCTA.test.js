import React from "react";
import LeadFormCTA from "../../src/components/LeadFormCTA";

test("Component LeadFormCTA Snapshot RenderedTree", () => {
  let wrapper = shallow(<LeadFormCTA />);
  expect(wrapper).toMatchSnapshot();
});
