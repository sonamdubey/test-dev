CREATE TABLE [dbo].[Con_Testimonial] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CatId]           NUMERIC (18)  NOT NULL,
    [CustomerName]    VARCHAR (100) NOT NULL,
    [CityId]          NUMERIC (18)  NULL,
    [Email]           VARCHAR (100) NULL,
    [ContactNo]       VARCHAR (100) NULL,
    [Comments]        VARCHAR (500) NOT NULL,
    [TestimonialDate] DATETIME      NOT NULL,
    [EntryDate]       DATETIME      NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_Con_Testimonial_IsActive] DEFAULT ((1)) NULL,
    [MakeId]          INT           NULL,
    [ModelId]         INT           NULL,
    [HostURL]         VARCHAR (250) NULL,
    [OriginalImgPath] VARCHAR (250) NULL,
    [StatusId]        INT           NULL,
    CONSTRAINT [PK_Con_Testimonial] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

