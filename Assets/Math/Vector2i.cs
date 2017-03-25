public class Vector2i {

    public int x;
    public int z;

    public Vector2i(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public override int GetHashCode() {
        return this.x.GetHashCode() ^ this.z.GetHashCode();
    }

    public override bool Equals(object obj) {
        Vector2i other = obj as Vector2i;

        if (other == null) {
            return false;
        }

        return this.x == other.x && this.z == other.z;
    }

}
