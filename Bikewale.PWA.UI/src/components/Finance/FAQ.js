import React from 'react'

import Accordion from '../Shared/Accordion'

const FAQ = () => {
  return (
    <div>
      <div className="finance-faq__head">
        <div className="finance-faq__image"></div>
      </div>
      <div className="finance-faq__body">
        <h2 className="finance-faq__heading">Frequently Asked Questions</h2>
        <div className="finance-faq__list">
          <Accordion closeable={true}>
            <div data-trigger="Why Should I Choose a Two-Wheeler Loan?">
              <div className="faq-list-item__content">1. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum finibus purus at mi auctor vestibulum. Vivamus vitae tortor nulla. Aenean sagittis dui accumsan leo porttitor, vel feugiat metus semper. Cras dapibus egestas eros, eget efficitur nunc. Vivamus sed est a ipsum scelerisque bibendum sed vel felis. Fusce hendrerit mollis tortor quis convallis. Suspendisse et leo tellus. Vestibulum tempor diam at ullamcorper aliquam. Duis lacinia neque quam, sed imperdiet nunc molestie a. Aenean ut metus ultrices lacus posuere pharetra vitae ut lacus. Morbi pretium arcu sed enim convallis gravida. Sed lobortis luctus porttitor. Vivamus gravida nunc velit, vel auctor massa tincidunt id. Nunc nisl arcu, eleifend ut ante et, blandit maximus risus. Donec est arcu, ultrices id est a, tincidunt sodales eros. Mauris a ligula consequat leo fringilla maximus sit amet in quam.</div>
            </div>
            <div data-trigger="How to Avail Two-Wheeler Loans?">
              <div className="faq-list-item__content">2. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum finibus purus at mi auctor vestibulum. Vivamus vitae tortor nulla. Aenean sagittis dui accumsan leo porttitor, vel feugiat metus semper. Cras dapibus egestas eros, eget efficitur nunc. Vivamus sed est a ipsum scelerisque bibendum sed vel felis. Fusce hendrerit mollis tortor quis convallis. Suspendisse et leo tellus. Vestibulum tempor diam at ullamcorper aliquam. Duis lacinia neque quam, sed imperdiet nunc molestie a. Aenean ut metus ultrices lacus posuere pharetra vitae ut lacus. Morbi pretium arcu sed enim convallis gravida. Sed lobortis luctus porttitor. Vivamus gravida nunc velit, vel auctor massa tincidunt id. Nunc nisl arcu, eleifend ut ante et, blandit maximus risus. Donec est arcu, ultrices id est a, tincidunt sodales eros. Mauris a ligula consequat leo fringilla maximus sit amet in quam.</div>
            </div>
            <div data-trigger="What is a CIBIL Score and Report?">
              <div className="faq-list-item__content">3. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum finibus purus at mi auctor vestibulum. Vivamus vitae tortor nulla. Aenean sagittis dui accumsan leo porttitor, vel feugiat metus semper. Cras dapibus egestas eros, eget efficitur nunc. Vivamus sed est a ipsum scelerisque bibendum sed vel felis. Fusce hendrerit mollis tortor quis convallis. Suspendisse et leo tellus. Vestibulum tempor diam at ullamcorper aliquam. Duis lacinia neque quam, sed imperdiet nunc molestie a. Aenean ut metus ultrices lacus posuere pharetra vitae ut lacus. Morbi pretium arcu sed enim convallis gravida. Sed lobortis luctus porttitor. Vivamus gravida nunc velit, vel auctor massa tincidunt id. Nunc nisl arcu, eleifend ut ante et, blandit maximus risus. Donec est arcu, ultrices id est a, tincidunt sodales eros. Mauris a ligula consequat leo fringilla maximus sit amet in quam.</div>
            </div>
            <div data-trigger="Passport-size photographs, at least 2 to 6 are recommended to be kept handy?">
              <div className="faq-list-item__content">4. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum finibus purus at mi auctor vestibulum. Vivamus vitae tortor nulla. Aenean sagittis dui accumsan leo porttitor, vel feugiat metus semper. Cras dapibus egestas eros, eget efficitur nunc. Vivamus sed est a ipsum scelerisque bibendum sed vel felis. Fusce hendrerit mollis tortor quis convallis. Suspendisse et leo tellus. Vestibulum tempor diam at ullamcorper aliquam. Duis lacinia neque quam, sed imperdiet nunc molestie a. Aenean ut metus ultrices lacus posuere pharetra vitae ut lacus. Morbi pretium arcu sed enim convallis gravida. Sed lobortis luctus porttitor. Vivamus gravida nunc velit, vel auctor massa tincidunt id. Nunc nisl arcu, eleifend ut ante et, blandit maximus risus. Donec est arcu, ultrices id est a, tincidunt sodales eros. Mauris a ligula consequat leo fringilla maximus sit amet in quam.</div>
            </div>
          </Accordion>
        </div>
      </div>
    </div>
  );
}

export default FAQ
