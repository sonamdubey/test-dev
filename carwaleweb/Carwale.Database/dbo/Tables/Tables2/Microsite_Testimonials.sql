CREATE TABLE [dbo].[Microsite_Testimonials] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [DealerId]       INT           NOT NULL,
    [Testimonials]   VARCHAR (500) NOT NULL,
    [CustomerName]   VARCHAR (50)  NOT NULL,
    [IsActive]       BIT           CONSTRAINT [DF_NCD_Testmonials_IsActive] DEFAULT ((1)) NOT NULL,
    [IsApproved]     BIT           CONSTRAINT [DF_Microsite_Testimonials_IsApproved] DEFAULT ((1)) NOT NULL,
    [CustomersNo]    VARCHAR (12)  NULL,
    [CustomersEmail] VARCHAR (50)  NULL,
    CONSTRAINT [PK_NCD_Testmonials] PRIMARY KEY CLUSTERED ([Id] ASC)
);

