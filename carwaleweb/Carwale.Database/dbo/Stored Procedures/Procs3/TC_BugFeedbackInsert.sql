IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_BugFeedbackInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_BugFeedbackInsert]
GO

	-- =============================================

-- Author:		Binu

-- Create date: 30 May,2012

-- Description:	Added @TC_BugFeedbackCategory_Id parameter

-- =============================================

-- Author:		Surendra

-- Create date: 22nd Nov,2011

-- Description:	Adding Bugs raised by Customer(Dealer)

-- Modified : Afrose BugScreenShotStatusId, @RetVal Output

-- =============================================

CREATE PROCEDURE [dbo].[TC_BugFeedbackInsert]

(

@BranchId BIGINT,

@CustomerFeedback VARCHAR(400),

@FilePath VARCHAR(100)=NULL,

@HostUrl VARCHAR(50),

@UserAgent VARCHAR(50),

@ClientOS VARCHAR(50),

@TC_BugFeedbackCategoryId TINYINT,

@BugScreenShotStatusId INT,

@RetVal INT OUTPUT

)

AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	SET NOCOUNT ON;

	INSERT INTO TC_BugFeedback(BranchId,CustomerFeedback,FilePath,HostUrl,UserAgent,ClientOS, TC_FeedbackCategoryId,BugScreenShotStatusId, OriginalImgPath )

	VALUES(@BranchId,@CustomerFeedback,@FilePath,@HostUrl,@UserAgent,@ClientOS, @TC_BugFeedbackCategoryId, @BugScreenShotStatusId, @FilePath)



	SET @RetVal = SCOPE_IDENTITY()

END










/****** Object:  StoredProcedure [dbo].[Microsite_SelectStockImages]    Script Date: 8/14/2015 11:49:16 AM ******/
SET ANSI_NULLS ON
