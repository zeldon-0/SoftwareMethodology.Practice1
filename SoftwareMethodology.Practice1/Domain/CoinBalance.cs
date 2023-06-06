namespace SoftwareMethodology.Practice1.Domain;

public class CoinBalance
{
    public string CountryMotif { get; }
    public int Amount { get; private set; }
    private int _pendingAmount;
    
    public CoinBalance(string countryMotif, int amount)
    {
        CountryMotif = countryMotif;
        Amount = amount;
        _pendingAmount = amount;
    }

    public void Increase(int transactionAmount) => _pendingAmount += transactionAmount;

    public void Decrease(int transactionAmount) => _pendingAmount -= transactionAmount;

    public void CommitTransaction() => Amount = _pendingAmount;
}
