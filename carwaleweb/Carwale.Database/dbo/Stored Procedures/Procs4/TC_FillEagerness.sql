IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FillEagerness]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FillEagerness]
GO

	--	Author	:	Sachin Bharti(18th March 2013)
--	Purpose	:	To get eagerness
CREATE PROCEDURE [dbo].[TC_FillEagerness]
AS
BEGIN
	SELECT TIS.TC_InquiryStatusId Value , TIS.Status AS Text FROM TC_InquiryStatus TIS WITH(NOLOCK) 
    WHERE TIS.IsActive = 1 ORDER BY TIS.TC_InquiryStatusId ASC
END
