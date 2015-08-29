CREATE proc dbo.GetActiveRestrictionCount
as
begin
	select	count(1) as CNT
	from	IISRestriction
	where	Enabled = 1
end