CREATE TABLE [dbo].[Classified_ReportListingReasons] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Reason]    VARCHAR (250) NOT NULL,
    [ForDealer] BIT           NULL,
    CONSTRAINT [PK_ClassReListRs] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Whether the reason is specfic only to Dealer Classified Listings Or to Individual Classified Listing. 1->Dealer Listing, 0-> Individual Listing, null-> Any listing', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Classified_ReportListingReasons', @level2type = N'COLUMN', @level2name = N'ForDealer';

