IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetInquirySource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetInquirySource]
GO
	
-- Author:        Surendra
-- Create date: 12 Jan 2012
-- Description:    This procedure is used to get Inquiry Source
-- Modified By : Tejashree Patil on 4 Feb 2013 at 11 am; Added condition to display Inquiry Source in dropdown.
-- Modified By: Nilesh Utture on 12th September, 2013 Added MakeId in Select Field
-- Modified BY: Vivek Gupta on 25-11-2015, added @CustomerId to show carwale or bikewale source in case of the lead was already from carwale or bikewale
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetInquirySource]
	@ApplicationId SMALLINT,
	@CustomerId INT = NULL
AS
BEGIN     
	IF @ApplicationId = 1
		BEGIN    
			SELECT  Id, Source
            FROM    TC_InquirySource WITH(NOLOCK)
            WHERE    IsActive=1 AND (IsVisibleCW=1 OR Id = (SELECT TC_InquirySourceId FROM TC_CustomerDetails WITH(NOLOCK) where id = @CustomerId))
            ORDER BY Source
		END
	ELSE
        BEGIN
			SELECT    Id, Source
            FROM    TC_InquirySource WITH(NOLOCK)
            WHERE    IsActive=1 AND (IsVisibleBW=1  OR Id = (SELECT TC_InquirySourceId FROM TC_CustomerDetails WITH(NOLOCK) where id = @CustomerId))
            ORDER BY Source
	      END
END









