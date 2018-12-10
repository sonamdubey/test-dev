CREATE TABLE [dbo].[ConsumerInquiriesViewable] (
    [ConsumerType]   SMALLINT     NOT NULL,
    [CustomerId]     NUMERIC (18) NOT NULL,
    [CarId]          NUMERIC (18) NOT NULL,
    [Type]           CHAR (1)     CONSTRAINT [DF_ConsumerInquiriesViewable_Type] DEFAULT ('P') NOT NULL,
    [ViewedDateTime] DATETIME     NOT NULL,
    [OpenedBy]       NUMERIC (18) NOT NULL,
    [OpenedByCat]    SMALLINT     NOT NULL,
    CONSTRAINT [PK_ConsumerInquiriesViewable] PRIMARY KEY CLUSTERED ([ConsumerType] ASC, [CustomerId] ASC, [CarId] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_ConsumerInquiriesViewable_ConsumerTypeCarId]
    ON [dbo].[ConsumerInquiriesViewable]([ConsumerType] ASC, [CarId] ASC);

