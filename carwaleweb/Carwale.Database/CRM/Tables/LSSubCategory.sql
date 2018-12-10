CREATE TABLE [CRM].[LSSubCategory] (
    [SubCategoryId] INT           NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [MakeId]        INT           NULL,
    [PriceFrom]     NUMERIC (18)  NULL,
    [PriceTo]       NUMERIC (18)  NULL,
    [SourceId]      INT           NULL,
    [TierId]        INT           NULL,
    [Descr]         VARCHAR (200) NULL,
    [CategoryId]    INT           NOT NULL,
    [Value]         FLOAT (53)    NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_LSSubCategory_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedOn]     DATETIME      CONSTRAINT [DF_LSSubCategory_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [UpdatedOn]     DATETIME      NULL,
    [UpdatedBy]     INT           NOT NULL,
    CONSTRAINT [PK_LSSubCategory] PRIMARY KEY CLUSTERED ([SubCategoryId] ASC)
);


GO


CREATE TRIGGER [CRM].[TRG_BU_LSSubCategory]
on [CRM].[LSSubCategory]
after Update
as
begin

If update(Value)
  insert into [CRM].[LSSubCategoryLogs]
  ( SubCategoryId,
	[Value]  ,
	MakeId,
	PriceFrom,
	PriceTo,
	SourceId,
	TierId,	
	[UpdatedOn],
	[UpdatedBy] 
  )
 select SubCategoryId,
        Value,
        MakeId,
		PriceFrom,
		PriceTo,
		SourceId,
		TierId,
		GETDATE(),
		UpdatedBy
 from Deleted
end


