CREATE TABLE [dbo].[NCD_Enquiries] (
    [enq_id]           INT           IDENTITY (1, 1) NOT NULL,
    [enq_dealer_id]    INT           NOT NULL,
    [enq_city_id]      INT           NULL,
    [enq_version_id]   INT           NULL,
    [enq_buy_plan]     VARCHAR (30)  NULL,
    [enq_cname]        VARCHAR (50)  NOT NULL,
    [enq_email]        VARCHAR (100) NOT NULL,
    [enq_mobile]       VARCHAR (10)  NULL,
    [enq_req_type]     TINYINT       NOT NULL,
    [enq_req_datetime] DATETIME      NOT NULL,
    [enq_deleted]      CHAR (1)      CONSTRAINT [DF_NCD_PriceQuote_pq_deleted] DEFAULT ('N') NOT NULL,
    [enq_query]        TEXT          NULL,
    CONSTRAINT [PK_NCD_PriceQuote] PRIMARY KEY CLUSTERED ([enq_id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Price Quote,2=Test Drive,3=Query', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_Enquiries', @level2type = N'COLUMN', @level2name = N'enq_id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Price Quote,2=Test Drive,3=Customer Query,4=Loan Query,5=Call me now', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'NCD_Enquiries', @level2type = N'COLUMN', @level2name = N'enq_req_type';

