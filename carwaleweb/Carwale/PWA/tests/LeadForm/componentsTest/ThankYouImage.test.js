import React from "react";
import ThankYouImage from "../../../src/LeadForm/Components/ThankYouImage";

test("Component ThankYouImage Snapshot RenderedTree", () => {
  let wrapper = shallow(<ThankYouImage />);
  expect(wrapper).toMatchSnapshot();
});
