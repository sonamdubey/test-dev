CREATE TABLE [dbo].[TC_PurchaseInquiries] (
    [Id]              NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [BranchId]        NUMERIC (18)  NOT NULL,
    [CustomerId]      NUMERIC (18)  NOT NULL,
    [StockId]         NUMERIC (18)  NULL,
    [Comments]        VARCHAR (500) NULL,
    [InterestedIn]    VARCHAR (500) NULL,
    [InquiryStatusId] INT           NULL,
    [SourceId]        INT           NOT NULL,
    [FollowUp]        DATETIME      NULL,
    [RequestDateTime] DATETIME      NOT NULL,
    [IsActive]        BIT           CONSTRAINT [DF_TC_PurchaseInquiries_IsActive] DEFAULT ((1)) NOT NULL,
    [FollowupComment] VARCHAR (200) NULL,
    [IsActionTaken]   BIT           CONSTRAINT [DF_TC_PurchaseInquiries_IsActionTaken] DEFAULT ((0)) NOT NULL,
    [ModifiedDate]    DATETIME      NULL,
    [EntryDate]       DATETIME      CONSTRAINT [DF__TC_Purcha__Entry__31DCEDEE] DEFAULT (getdate()) NULL,
    [ModifiedBy]      INT           NULL,
    [IsMigrated]      BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TC_PurchaseInquiries] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
-- =============================================
-- Author:		Vikas
-- Create date: 31/05/2011
-- Description:	To Update the response count based on the source of the Inquiry
-- =============================================
CREATE TRIGGER [dbo].[TR_UpdateResponseCount] 
   ON  [dbo].[TC_PurchaseInquiries] 
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	Declare @StockId BIGINT = Null
	Declare @SourceId SmallInt
	
	Select Top 1 @StockId = StockId, @SourceId = SourceId From Inserted
	If @StockId Is Not Null
	Begin 
		/*	SourceId = 1 Implies that the Inquiry came through CarWale.
		
			CWResponseCount: This value indicates the responses on the Stock from CarWale as Source ( SourceId = 1 )
			TCResponseCount: This value indicates the responses on the Stock from Sources other than CarWale ( SourceId <> 1 )
			
			If Inquiry for a particular Car in stock came via CarWale, Increment value for CWResponseCount 
			and Decrement value for TCResponseCount ( if TCResponseCount is not '0' ) And vice-versa.
		*/
		
		If NOT EXISTS(SELECT StockId FROM TC_StockAnalysis WHERE StockId = @StockId )
		BEGIN
			INSERT INTO TC_StockAnalysis(StockId, CWResponseCount, TCResponseCount) VALUES(@StockId, 0, 0)
		END
		
		If ( @SourceId = 1 ) -- CarWale as source
		Begin
			-- Update CWResponseCount to Table.
			Update TC_StockAnalysis Set CWResponseCount = CWResponseCount + 1 Where StockId = @StockId
		End
		Else -- Any other source
		Begin
			-- Update TCResponseCount to Table.
			Update TC_StockAnalysis Set TCResponseCount = TCResponseCount + 1 Where StockId = @StockId
		End
	End
END
