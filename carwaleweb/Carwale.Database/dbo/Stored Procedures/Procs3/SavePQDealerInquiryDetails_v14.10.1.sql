IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SavePQDealerInquiryDetails_v14]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SavePQDealerInquiryDetails_v14]
GO

	


-- =============================================
-- Author:		Ashish Verma
-- Create date: 07/08/2014
-- Description:	To save PQ dealer add inquiry details before calling to api
-- exec [dbo].[SavePQDealerInquiryDetails] 35,1,""
-- Modified by : Vinayak on 15-10-2014 Added columns Name,Email and Mobile:for version SavePQDealerInquiryDetails_v14.10.1
-- =============================================
CREATE PROCEDURE [dbo].[SavePQDealerInquiryDetails_v14.10.1] 
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC
	,@DealerId INT
	,@LeadClickSource INT
	,@DealerLeadBusinessType INT
	,@ResponseId NUMERIC OUTPUT
	,@Name varchar(100)=NULL
	,@Email varchar(100)=NULL
	,@Mobile varchar(100)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO PQDealerAdLeads(PQId,DealerId,LeadClickSource,DealerLeadBusinessType,Name,Email,Mobile) values (@PQId,@DealerId,@LeadClickSource,@DealerLeadBusinessType,@Name,@Email,@Mobile)

	SET @ResponseId = SCOPE_IDENTITY();
END

