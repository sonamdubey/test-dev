IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertDealerVisitFeedback]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertDealerVisitFeedback]
GO
	
--SELECT ID, Name FROM  States Where IsDeleted = 0 ORDER BY Name 
----AND ID IN(SELECT StateId FROM Cities WHERE IsDeleted = 0)
--ORDER BY Name 

--SELECT * FROM  DealerVisitFeedback

CREATE PROCEDURE [dbo].[InsertDealerVisitFeedback]
( 
	@DealerId		NUMERIC,
	@VisitDate		DATETIME,
	@Owner			NVARCHAR(100),
	@StockAtDealer  NUMERIC,
	@StockOnCarwale NUMERIC,
	@WithOutPicOnCW NUMERIC,
	@DlrRespondent  NVARCHAR(100),
	@Designation	NVARCHAR(100),
	@DlrRespMobile	NVARCHAR(50),
	@BuyerInquiry	SMALLINT,
	@CarsSold		SMALLINT,
	@SellLeads		SMALLINT,
	@CWSupport		SMALLINT,
	@OAllSatis		SMALLINT,
	@UseCWSW		SMALLINT,
	@IsUseful		SMALLINT,
	@Suggestion		NVARCHAR(500),
	@Comments		NVARCHAR(500),
	@ExecutiveId	NUMERIC,
	@EntryDate		DATETIME = GETDATE,
	@FeedbackId		NUMERIC OUTPUT
)

AS

BEGIN

	INSERT INTO [DealerVisitFeedback] 
		(DealerId, VisitDate, [Owner], StockAtDealer,StockOnCarwale, WithOutPicOnCW, DlrRespondent, Designation, DlrRespMobile, 
		 BuyerInquiry, CarsSold, SellLeads, CWSupport, OAllSatis, UseCWSW, IsUseful, Suggestion, Comments, ExecutiveId, EntryDate)
	VALUES
		(@DealerId, @VisitDate, @Owner, @StockAtDealer, @StockOnCarwale, @WithOutPicOnCW, @DlrRespondent, @Designation, @DlrRespMobile,
		@BuyerInquiry, @CarsSold, @SellLeads, @CWSupport, @OAllSatis, @UseCWSW, @IsUseful, @Suggestion, @Comments, @ExecutiveId, @EntryDate)

	SET @FeedbackId	= SCOPE_IDENTITY()		
END
