CREATE TABLE [dbo].[Dealer_Testimonial] (
    [ID]          NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [DealerId]    NUMERIC (18)   NOT NULL,
    [UserName]    NVARCHAR (100) NOT NULL,
    [Testimonial] NVARCHAR (500) NOT NULL,
    [UpdatedOn]   DATETIME       NOT NULL,
    [UpdatedBy]   NUMERIC (18)   NOT NULL,
    [IsActive]    BIT            CONSTRAINT [DF_Dealer_Testimonial_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Dealer_Testimonial] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1=Active, 0 = InActive', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Dealer_Testimonial', @level2type = N'COLUMN', @level2name = N'IsActive';

